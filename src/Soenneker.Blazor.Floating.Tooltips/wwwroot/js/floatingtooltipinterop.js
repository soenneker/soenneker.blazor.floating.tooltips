const cleanups = new Map();
const timeouts = new Map();
const handlersByTooltipId = new Map();

export async function create(id, optionsJson) {
    const options = JSON.parse(optionsJson);
    options.placement ||= "top";

    const anchorId = "anchor-" + id;
    const tooltipId = "tooltip-" + id;

    const anchor = document.getElementById(anchorId);
    const tooltip = document.getElementById(tooltipId);

    if (!anchor || !tooltip) {
        console.warn('Reference or tooltip element not found.', {
            anchorId,
            tooltipId,
            anchorFound: !!anchor,
            tooltipFound: !!tooltip
        });
        return;
    }

    let reference = anchor.querySelector("[data-tooltip-anchor]") ?? anchor.firstElementChild;

    if (!reference) {
        reference = anchor;
        anchor.style.display = "inline-block";
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

    if (options.maxWidth && options.maxWidth > 0) {
        const textEl = tooltip.querySelector(".tooltip-text");

        if (textEl && options.maxWidth && options.maxWidth > 0) {
            textEl.style.maxWidth = `${options.maxWidth}px`;
        }
    }

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
        clearTimeout(timeouts.get(`hide-${id}`));

        const showDelay = options.showDelay || 0;
        const showTimeout = setTimeout(() => {
            tooltip.classList.add("visible");

            cleanup = window.FloatingUIDOM.autoUpdate(reference, tooltip, async () => {
                try {
                    const {
                        x, y,
                        middlewareData,
                        placement: resolvedPlacement
                    } = await window.FloatingUIDOM.computePosition(reference, tooltip, {
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

        timeouts.set(`show-${id}`, showTimeout);
    };

    const hideTooltip = () => {
        clearTimeout(timeouts.get(`show-${id}`));

        const hideDelay = options.hideDelay || 0;
        const hideTimeout = setTimeout(() => {
            tooltip.classList.remove("visible");
            if (cleanup) {
                cleanup();
                cleanup = null;
            }
        }, hideDelay);

        timeouts.set(`hide-${id}`, hideTimeout);
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

        reference.addEventListener('mouseenter', onEnter);
        reference.addEventListener('mouseleave', onLeave);

        cleanups.set(anchorId, () => {
            reference.removeEventListener('mouseenter', onEnter);
            reference.removeEventListener('mouseleave', onLeave);
            clearTimeout(timeouts.get(`show-${id}`));
            clearTimeout(timeouts.get(`hide-${id}`));
            if (cleanup) {
                cleanup();
                cleanup = null;
            }
        });
    } else {
        cleanups.set(anchorId, () => {
            clearTimeout(timeouts.get(`show-${id}`));
            clearTimeout(timeouts.get(`hide-${id}`));
            if (cleanup) {
                cleanup();
                cleanup = null;
            }
        });
    }

    handlersByTooltipId.set(id, {
        showTooltip,
        hideTooltip
    });

    observeAnchorRemoval(anchorId, id);
}

function observeAnchorRemoval(anchorId, tooltipId) {
    const target = document.getElementById(anchorId);
    if (!target || !target.parentNode || !cleanups.has(anchorId)) return;

    const observer = new MutationObserver((mutations) => {
        const targetRemoved = mutations.some(mutation =>
            Array.from(mutation.removedNodes).includes(target)
        );

        if (targetRemoved) {
            dispose(tooltipId);
            observer.disconnect();
        }
    });

    observer.observe(target.parentNode, { childList: true });
}

export function setCallbacks(id, dotNetRef) {
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

export function dispose(id) {
    const anchorId = "anchor-" + id;

    const cleanupFn = cleanups.get(anchorId);
    if (cleanupFn) {
        cleanupFn();
        cleanups.delete(anchorId);
    }

    clearTimeout(timeouts.get(`show-${id}`));
    clearTimeout(timeouts.get(`hide-${id}`));
    timeouts.delete(`show-${id}`);
    timeouts.delete(`hide-${id}`);

    handlersByTooltipId.delete(id);
}

export function show(id) {
    const handlers = handlersByTooltipId.get(id);
    if (handlers?.showTooltip) {
        handlers.showTooltip();
    } else {
        console.warn(`No showTooltip found for tooltip ID: ${id}`);
    }
}

export function hide(id) {
    const handlers = handlersByTooltipId.get(id);
    if (handlers?.hideTooltip) {
        handlers.hideTooltip();
    } else {
        console.warn(`No hideTooltip found for tooltip ID: ${id}`);
    }
}

export function toggle(id) {
    const tooltipId = "tooltip-" + id;
    const tooltip = document.getElementById(tooltipId);

    if (!tooltip) {
        console.warn(`Tooltip element not found for ID: ${id}`);
        return;
    }

    if (tooltip.classList.contains("visible")) {
        hide(id);
    } else {
        show(id);
    }
}
