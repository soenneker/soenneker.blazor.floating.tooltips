export class FloatingTooltipInterop {
    constructor() {
        this.cleanups = new Map();
        this.debug = false;
        this._timeouts = new Map();
    }

    async create(id, optionsJson) {
        const options = JSON.parse(optionsJson);
        options.placement ||= "top";

        const anchorId = "anchor-" + id;
        const tooltipId = "tooltip-" + id;

        const anchor = document.getElementById(anchorId);
        const tooltip = document.getElementById(tooltipId);

        if (!anchor || !tooltip) {
            console.warn('Anchor or tooltip element not found.', { anchorId, tooltipId });
            return;
        }

        if (!options?.enabled) {
            tooltip.style.display = 'none';
            return;
        }

        tooltip.classList.add("floating-tooltip");

        if (options.theme)
            tooltip.classList.add(`floating-tooltip--theme-${options.theme}`);

        if (options.animate)
            tooltip.classList.add("fade");

        if (options.interactive)
            tooltip.classList.add("interactive");

        if (options.maxWidth && options.maxWidth > 0)
            tooltip.style.maxWidth = `${options.maxWidth}px`;

        tooltip.dataset.placement = options.placement;

        if (options.showArrow && !tooltip.querySelector(".floating-tooltip-arrow")) {
            const arrow = document.createElement("div");
            arrow.classList.add("floating-tooltip-arrow");
            tooltip.appendChild(arrow);
        }

        const arrowEl = tooltip.querySelector(".floating-tooltip-arrow");

        let cleanup;

        const showTooltip = async () => {
            if (!options.enabled) return;
            clearTimeout(this._timeouts.get(`hide-${id}`));

            const showDelay = options.showDelay || 0;
            const showTimeout = setTimeout(() => {
                tooltip.classList.add("visible");

                cleanup = window.FloatingUIDOM.autoUpdate(anchor, tooltip, async () => {
                    try {
                        const {
                            x, y,
                            middlewareData,
                            placement: resolvedPlacement
                        } = await window.FloatingUIDOM.computePosition(anchor, tooltip, {
                            placement: options.placement,
                            middleware: [
                                window.FloatingUIDOM.offset(10),
                                window.FloatingUIDOM.flip(),
                                window.FloatingUIDOM.shift({ padding: 5 }),
                                ...(arrowEl ? [window.FloatingUIDOM.arrow({ element: arrowEl })] : [])
                            ]
                        });

                        Object.assign(tooltip.style, {
                            position: 'absolute',
                            left: `${x}px`,
                            top: `${y}px`
                        });

                        const [basePlacement, alignment = "center"] = resolvedPlacement.split("-");

                        // ✨ Apply alignment class to tooltip
                        tooltip.classList.remove("tooltip-align-start", "tooltip-align-end", "tooltip-align-center");
                        tooltip.classList.add(`tooltip-align-${alignment}`);

                        if (arrowEl && middlewareData?.arrow) {
                            const { x: ax, y: ay } = middlewareData.arrow;

                            arrowEl.style.left = '';
                            arrowEl.style.right = '';
                            arrowEl.style.top = '';
                            arrowEl.style.bottom = '';
                            arrowEl.style.marginLeft = '';
                            arrowEl.style.marginRight = '';

                            if (basePlacement === "top" || basePlacement === "bottom") {
                                arrowEl.style.left = ax != null ? `${ax}px` : '';
                            } else {
                                arrowEl.style.top = ay != null ? `${ay}px` : '';
                            }

                            const staticSide = {
                                top: 'bottom',
                                right: 'left',
                                bottom: 'top',
                                left: 'right'
                            }[basePlacement];

                            arrowEl.style[staticSide] = '-4px';
                        }
                    } catch (e) {
                        console.error("computePosition error", e);
                    }
                });
            }, showDelay);

            this._timeouts.set(`show-${id}`, showTimeout);
        };

        const hideTooltip = () => {
            clearTimeout(this._timeouts.get(`show-${id}`));

            const hideDelay = options.hideDelay || 0;
            const hideTimeout = setTimeout(() => {
                tooltip.classList.remove("visible");
                if (cleanup) {
                    cleanup();
                    cleanup = null;
                }
            }, hideDelay);

            this._timeouts.set(`hide-${id}`, hideTimeout);
        };

        if (!options.manualTrigger) {
            const onEnter = showTooltip;

            const onLeave = () => {
                if (options.interactive && tooltip) {
                    setTimeout(() => {
                        const stillHovered = tooltip.matches(':hover');
                        if (stillHovered) {
                            const leaveFromTooltip = (evt) => {
                                if (!anchor.contains(evt.relatedTarget)) {
                                    hideTooltip();
                                }
                            };

                            tooltip.addEventListener("mouseleave", leaveFromTooltip, { once: true });
                        } else {
                            hideTooltip();
                        }
                    }, 100);
                    return;
                }

                hideTooltip();
            };

            anchor.addEventListener('mouseenter', onEnter);
            anchor.addEventListener('mouseleave', onLeave);

            this.cleanups.set(anchorId, () => {
                anchor.removeEventListener('mouseenter', onEnter);
                anchor.removeEventListener('mouseleave', onLeave);
                clearTimeout(this._timeouts.get(`show-${id}`));
                clearTimeout(this._timeouts.get(`hide-${id}`));
                if (cleanup) {
                    cleanup();
                    cleanup = null;
                }
            });
        } else {
            this.cleanups.set(anchorId, () => {
                clearTimeout(this._timeouts.get(`show-${id}`));
                clearTimeout(this._timeouts.get(`hide-${id}`));
                if (cleanup) {
                    cleanup();
                    cleanup = null;
                }
            });
        }

        this[`__handlers_${id}`] = {
            showTooltip,
            hideTooltip
        };

        this.createObserver(anchorId);
    }

    createObserver(elementId) {
        const target = document.getElementById(elementId);
        if (!target || !target.parentNode || !this.cleanups.has(elementId)) return;

        const observer = new MutationObserver((mutations) => {
            const targetRemoved = mutations.some(mutation =>
                Array.from(mutation.removedNodes).includes(target)
            );

            if (targetRemoved) {
                this.dispose(elementId);
                observer.disconnect();
            }
        });

        observer.observe(target.parentNode, { childList: true });
    }

    setCallbacks(id, dotNetRef) {
        const anchorId = "anchor-" + id;
        const anchor = document.getElementById(anchorId);
        if (!anchor) return;

        anchor.addEventListener("mouseenter", () => {
            dotNetRef.invokeMethodAsync("InvokeOnShow").catch(console.error);
        });

        anchor.addEventListener("mouseleave", () => {
            dotNetRef.invokeMethodAsync("InvokeOnHide").catch(console.error);
        });
    }

    dispose(id) {
        const anchorId = "anchor-" + id;

        const cleanupFn = this.cleanups.get(anchorId);
        if (cleanupFn) {
            cleanupFn();
            this.cleanups.delete(anchorId);
        }

        clearTimeout(this._timeouts.get(`show-${id}`));
        clearTimeout(this._timeouts.get(`hide-${id}`));
        this._timeouts.delete(`show-${id}`);
        this._timeouts.delete(`hide-${id}`);

        delete this[`__handlers_${id}`];
    }

    show(id) {
        const handlers = this[`__handlers_${id}`];
        if (handlers?.showTooltip) {
            handlers.showTooltip();
        } else {
            console.warn(`No showTooltip found for tooltip ID: ${id}`);
        }
    }

    hide(id) {
        const handlers = this[`__handlers_${id}`];
        if (handlers?.hideTooltip) {
            handlers.hideTooltip();
        } else {
            console.warn(`No hideTooltip found for tooltip ID: ${id}`);
        }
    }

    toggle(id) {
        const tooltipId = "tooltip-" + id;
        const tooltip = document.getElementById(tooltipId);

        if (!tooltip) {
            console.warn(`Tooltip element not found for ID: ${id}`);
            return;
        }

        if (tooltip.classList.contains("visible")) {
            this.hide(id);
        } else {
            this.show(id);
        }
    }
}

window.FloatingTooltipInterop = new FloatingTooltipInterop();
