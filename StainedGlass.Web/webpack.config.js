const path = require('path');

module.exports = {
    entry: {
        admin: './src/admin/app.ts',
        front: './src/front/app.ts',
    },
    output: {
        filename: '[name]bundle.js',
        path: path.resolve(__dirname, 'wwwroot/js')
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: {
                    loader: 'ts-loader',
                    options: { 
                        configFile: 'tsconfig.admin.json'
                    }
                },
                include: path.resolve(__dirname, 'src/admin'),
            },
            {
                test: /\.ts$/,
                use: {
                    loader: 'ts-loader',
                    options: {
                        configFile: 'tsconfig.front.json'
                    }
                },
                include: path.resolve(__dirname, 'src/front'),
            }
        ]
    },
    devServer: {
        static: { 
            directory: path.join(__dirname, 'wwwroot/js') 
        },
        compress: true,
        port: 9000
    },
    mode: 'development'
};
