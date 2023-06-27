/** @type {import('next').NextConfig} */
const path = require('path')

const nextConfig = {
  reactStrictMode: false,
  webpackDevMiddleware: config => {
    config.watchOptions = {
      poll: 300,
      aggregateTimeout: 300,
    };
    return config;
  },
  images: {
    domains: ["m.media-amazon.com"],
  },
  sassOptions: {
    includePaths: [path.join(__dirname, 'styles')],
  },
  unstable_includeFiles: [
    'node_modules/next/dist/compiled/@edge-runtime/primitives/**/*.+(js|json)',
  ],
};


module.exports = nextConfig
