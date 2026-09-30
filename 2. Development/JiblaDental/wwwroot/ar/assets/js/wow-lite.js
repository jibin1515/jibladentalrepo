/*
 * Minimal replacement for WOW.js (same API used by the site: `new WOW().init()`), built on IntersectionObserver.
 * WOW.js walked and measured every .wow element on init (~70 ms of main-thread time on the home page);
 * this only registers the elements and lets the browser tell us when they scroll into view.
 * Honours data-wow-duration / data-wow-delay. The animation itself still comes from the animate.css class on the element.
 * Elements added later (e.g. slides cloned by owl.carousel) are picked up through a MutationObserver.
 */
(function () {
  'use strict';

  function reveal(el) {
    var duration = el.getAttribute('data-wow-duration');
    var delay = el.getAttribute('data-wow-delay');
    if (duration) el.style.animationDuration = duration;
    if (delay) el.style.animationDelay = delay;
    el.style.visibility = 'visible';
    el.style.animationName = '';
    el.classList.add('animated');
  }

  function WOW() {}

  WOW.prototype.init = function () {
    if (!('IntersectionObserver' in window)) return;

    var observer = new IntersectionObserver(
      function (entries) {
        entries.forEach(function (entry) {
          if (!entry.isIntersecting) return;
          observer.unobserve(entry.target);
          reveal(entry.target);
        });
      },
      { rootMargin: '0px 0px -8% 0px' },
    );

    function register(el) {
      if (el.classList.contains('animated')) return;
      el.style.visibility = 'hidden';
      el.style.animationName = 'none';
      observer.observe(el);
    }

    Array.prototype.forEach.call(document.querySelectorAll('.wow'), register);

    if ('MutationObserver' in window) {
      new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
          Array.prototype.forEach.call(mutation.addedNodes, function (node) {
            if (node.nodeType !== 1) return;
            if (node.classList.contains('wow')) register(node);
            Array.prototype.forEach.call(node.querySelectorAll('.wow'), register);
          });
        });
      }).observe(document.body, { childList: true, subtree: true });
    }
  };

  window.WOW = WOW;
})();
