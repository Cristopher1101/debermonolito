/**
 * Reduce al máximo el autocompletado del navegador (no es lo mismo que cookies HttpOnly).
 * Se ejecuta al cargar, tras postbacks parciales (ASP.NET AJAX) y ante nodos nuevos en el DOM.
 */
(function () {
    var skipInputTypes = /^(hidden|submit|button|reset|image)$/i;
    var debounceMs = 60;
    var timer = null;

    function applyAll() {
        var doc = document;
        var i, j, el, t, forms, els;

        try {
            forms = doc.querySelectorAll("form");
            for (i = 0; i < forms.length; i++) {
                forms[i].setAttribute("autocomplete", "off");
            }

            els = doc.querySelectorAll("input, textarea, select");
            for (j = 0; j < els.length; j++) {
                el = els[j];
                t = (el.type || "").toLowerCase();
                if (el.tagName === "INPUT" && skipInputTypes.test(t)) {
                    continue;
                }
                if (t === "password") {
                    el.setAttribute("autocomplete", "new-password");
                } else {
                    el.setAttribute("autocomplete", "off");
                }
            }
        } catch (e) { /* ignore */ }
    }

    function scheduleApply() {
        if (timer) {
            window.clearTimeout(timer);
        }
        timer = window.setTimeout(function () {
            applyAll();
            timer = null;
        }, debounceMs);
    }

    function hookMutationObserver() {
        if (!document.body || typeof MutationObserver === "undefined") {
            return;
        }
        var obs = new MutationObserver(function (mutations) {
            var k, m;
            for (k = 0; k < mutations.length; k++) {
                m = mutations[k];
                if (m.addedNodes && m.addedNodes.length) {
                    scheduleApply();
                    return;
                }
            }
        });
        obs.observe(document.body, { childList: true, subtree: true });
    }

    function hookPageRequestManager() {
        if (typeof Sys === "undefined" || !Sys.WebForms || !Sys.WebForms.PageRequestManager) {
            return;
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (!prm) {
            return;
        }
        prm.add_endRequest(function () {
            applyAll();
        });
    }

    function init() {
        applyAll();
        hookMutationObserver();
        hookPageRequestManager();
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }
})();
