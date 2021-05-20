(function (window) {
    window.__env = window.__env || {};
    window.__env.identityUrl = "/gateway/",
    window.__env.apiGatewayUrl = "/gateway/",
    window.__env.modo = "normal", // "avancado",
    window.__env.client = {
        id: "SISGP.Web",
        secret: 'secret',
        scope: "SISGP.ProgramaGestao"
    }
}(this));
