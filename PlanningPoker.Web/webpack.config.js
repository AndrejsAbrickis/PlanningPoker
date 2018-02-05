const webpack = require("webpack");

module.exports = {
    context: __dirname + "/App",
    entry: {
        app: "./main.ts"
    },
    output: {
        path: __dirname + "/wwwroot/js",
        filename: "[name].bundle.js"
    },
    resolve: {
        extensions: ['.ts', '.js', '.vue', '.json'],
        alias: {
            'vue$': 'vue/dist/vue',
        }
    },
    module: {
        loaders: [
            {
                test: /\.ts$/,
                exclude: /node_modules|vue\/src/,
                use: [{
                    loader: 'ts-loader',
                    options: {
                        appendTsSuffixTo: [/\.vue$/]
                    }
                }]
            },
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