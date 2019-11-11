import axios from "axios";
const BASE_URL = "https://scmanmock.azurewebsites.net/";

export function getHttpClient() {
    return getClientInternal(BASE_URL);
}

function getClientInternal(baseUrl) {
    var client = axios.create({
        baseURL: baseUrl,
        timeout: 5000,
    });

    return client;
}