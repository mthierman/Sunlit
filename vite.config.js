import { defineConfig } from "vite";
import url from "url";

export default defineConfig(({ command, mode }) => {
    if (command === "serve") {
        const cert = url.fileURLToPath(
            new URL("../../.cert/localhost.pfx", import.meta.url)
        );
        const passphrase = "localhost";
        return {
            root: "./src/",
            base: "./",
            server: {
                port: 8000,
                https: {
                    pfx: cert,
                    passphrase: passphrase,
                },
            },
            preview: {
                port: 8000,
                https: {
                    pfx: cert,
                    passphrase: passphrase,
                },
            },
        };
    } else {
        return {
            root: "./src/",
            base: "./",
            build: {
                outDir: "../dist/",
                target: "esnext",
                emptyOutDir: true,
            },
        };
    }
});
