const API_BASE = "/api";

function getToken() {
  return localStorage.getItem("token");
}

async function request(path, options = {}) {
  const headers = options.headers ? { ...options.headers } : {};

  // JSON body esetén
  if (options.json) {
    headers["Content-Type"] = "application/json";
    options.body = JSON.stringify(options.json);
    delete options.json;
  }

  // Auth header, ha van token
  const token = getToken();
  if (token) headers["Authorization"] = `Bearer ${token}`;

  const res = await fetch(`${API_BASE}${path}`, { ...options, headers });

  // Próbáljuk JSON-ként olvasni, ha lehet
  let data = null;
  const contentType = res.headers.get("content-type") || "";
  if (contentType.includes("application/json")) {
    data = await res.json().catch(() => null);
  } else {
    data = await res.text().catch(() => null);
  }

  if (!res.ok) {
    const msg = (data && (data.message || data.error)) || `Hiba (${res.status})`;
    const err = new Error(msg);
    err.status = res.status;
    err.data = data;
    throw err;
  }

  return data;
}

// AUTH
export async function register(username, password) {
  // backend: POST /api/auth/register
  return request("/auth/register", { method: "POST", json: { username, password } });
}

export async function login(username, password) {
  // backend: POST /api/auth/login -> { token, user }
  const data = await request("/auth/login", { method: "POST", json: { username, password } });
  if (data?.token) localStorage.setItem("token", data.token);
  return data;
}

export async function me() {
  // backend: GET /api/auth/me
  return request("/auth/me", { method: "GET" });
}

export function logoutLocal() {
  localStorage.removeItem("token");
}

// CONTACTS
export async function createContact({ name, email, message }) {
  return request("/contacts", { method: "POST", json: { name, email, message } });
}

export async function getContacts() {
  return request("/contacts", { method: "GET" });
}

// DOWNLOADS
export async function getDownloads() {
  return request("/downloads", { method: "GET" });
}

// TRAINS (GET + POST)
export async function getTrains() {
  return request("/trains", { method: "GET" });
}

export async function createTrain({ name, user_id }) {
  // ha a backend req.user alapján dolgozik, a user_id nem kell, de nem baj ha elküldjük
  return request("/trains", { method: "POST", json: { name, user_id } });
}