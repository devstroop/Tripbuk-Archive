const path = require('path');

module.exports = {
    entry: './index.js',
    output: {
        filename: 'tripbuk-node-bundle.js',
        path: path.resolve(__dirname, 'wwwroot/js'),
    },
    mode: 'production',
};