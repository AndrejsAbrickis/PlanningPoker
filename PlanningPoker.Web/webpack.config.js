const webpack = require("webpack");

module.exports = {
    context: __dirname + "/App",
    entry: {
        app: "./main.js"
    },
    output: {
        path: __dirname + "/wwwroot/js",
        filename: "[name].bundle.js"
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue',
        }
    },
    module: {
        loaders: [
            {
                test: /\.vue$/,
                loader: 'vue-loader',
            },
            {
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/,
            }
        ]
    },
    plugins: [
    new webpack.optimize.CommonsChunkPlugin({
      name: ["manifest"],
      minChunks: Infinity,
    })
    ]
};