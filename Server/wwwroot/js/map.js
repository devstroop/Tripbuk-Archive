(function (global) {
    class Map {
        constructor(mapId, defaultLat, defaultLng, defaultZoom, theme) {
            this.mapId = mapId;
            this.defaultLatLng = L.latLng(defaultLat, defaultLng);
            this.defaultZoom = defaultZoom;
            this.map = null;
            this.latLngs = [];
            this.polygons = [];
            this.markers = [];
            this.lines = [];
            this.theme = theme;
        }
        // Theme options
        /*
            light_all,
            dark_all,
            light_nolabels,
            light_only_labels,
            dark_nolabels,
            dark_only_labels,
            rastertiles/voyager,
            rastertiles/voyager_nolabels,
            rastertiles/voyager_only_labels,
            rastertiles/voyager_labels_under
        */
        subdomain() {
            return 'abcd'.charAt(Math.floor(Math.random() * 'abcd'.length));
        }
        async initMap() {
            if (!this.map) {
                this.map = L.map(this.mapId).setView(this.defaultLatLng, this.defaultZoom);
                
                L.tileLayer(`https://${this.subdomain()}.basemaps.cartocdn.com/${this.theme}/{z}/{x}/{y}${(L.Browser.retina ? '@2x.png' : '.png')}`, {
                    attribution:'Maps | &copy; <a target="_blank" href="https://devstroop.com">Devstroop Technologies</a>',
                    subdomains: 'abcd',
                    maxZoom: 20,
                    minZoom: 0
                }).addTo(this.map);

                // L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
                //     maxZoom: 19,
                //     attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
                // }).addTo(this.map);
            }
        }

        addPolygon(data, content, color) {

            const polygon = L.polygon(eval(data)).bindTooltip(content).addTo(this.map);
            polygon.setStyle({
                color: color,
                opacity: 0.8,
                fillColor: color,
                fillOpacity: 0.35,
            });
            this.polygons.push(polygon);
            this.latLngs.push(polygon.getBounds().getCenter());
        }

        addIcon(lat, lng,iconUrl,content) {
            const icon = L.icon({
                iconUrl: iconUrl,
                //iconSize: [38, 95],
                iconAnchor: [20, 50],
            });

            const marker = L.marker([lat, lng], { icon }).bindTooltip(content).addTo(this.map);
            this.markers.push(marker);
            this.latLngs.push(marker);
        }
        addLine(latLngs, color,content) {
            const line = L.polyline(latLngs, {
                color: color,
                weight: 4,
                opacity: 0.7,
            }).bindTooltip(content).addTo(this.map);
            this.lines.push(line);
            this.latLngs.push(line.getBounds().getCenter());
        }
        clearMap() {
            this.polygons.forEach((polygon) => polygon.remove());
            this.polygons = [];
            this.markers.forEach((marker) => marker.remove());
            this.markers = [];
            this.lines.forEach((line) => line.remove());
            this.lines = [];
            this.latLngs = [];
        }

        centerMap() {
            if (this.latLngs.length > 0) {
                const bounds = L.latLngBounds(this.latLngs);
                this.map.fitBounds(bounds);
            } else {
                this.map.setView(this.defaultLatLng, this.defaultZoom);
            }
        }

        changeTheme(theme) {
            this.theme = theme;
            this.map.eachLayer((layer) => {
                if (layer instanceof L.TileLayer) {
                    layer.setUrl(`https://${this.subdomain()}.basemaps.cartocdn.com/${this.theme}/{z}/{x}/{y}${(L.Browser.retina ? '@2x.png' : '.png')}`);
                }
            });
        }
    }

    function loadLeaflet() {
        return new Promise((resolve, reject) => {
            const leafletCss = document.createElement('link');
            leafletCss.rel = 'stylesheet';
            leafletCss.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
            document.head.appendChild(leafletCss);

            const leafletScript = document.createElement('script');
            leafletScript.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
            leafletScript.async = true;
            document.body.appendChild(leafletScript);

            leafletScript.onload = () => resolve();
            leafletScript.onerror = () => reject(new Error("Failed to load Leaflet.js"));
        });
    }

    global.MapLibrary = {
        async createMap(mapId, lat, lng, zoom, theme = 'light_all') {
            await loadLeaflet();
            const mapInstance = new Map(mapId, lat, lng, zoom, theme);
            await mapInstance.initMap();
            return mapInstance;
        },
        addPolygonToMap(mapInstance, data, color, content="") {
            mapInstance.addPolygon(data, content, color);
        },
        addIconToMap(mapInstance, lat, lng, iconUrl, content = "") {
            mapInstance.addIcon(lat, lng, iconUrl, content);
        }
        ,
        addLineToMap(mapInstance, latLngs, color,content = "") {
            mapInstance.addLine(latLngs, color, content);
        },
        clearMap(mapInstance) {
            mapInstance.clearMap();
        },
        centerMap(mapInstance) {
            mapInstance.centerMap();
        },
        changeTheme(mapInstance, theme) {
            mapInstance.changeTheme(theme);
        }
    };
})(window);