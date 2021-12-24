importScripts("https://storage.googleapis.com/workbox-cdn/releases/3.6.3/workbox-sw.js")
const cacheName = 'cache-v2.1';
workbox.routing.registerRoute(/.*\.(?:js|css)/, new workbox.strategies.NetworkFirst({
    cacheName
}));
workbox.routing.registerRoute('/', new workbox.strategies.NetworkFirst({

    cacheName

}));
