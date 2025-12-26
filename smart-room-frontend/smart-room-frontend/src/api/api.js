import axios from "axios";

const API_URL = "https://localhost:7185/api"; // IP твого Backend

export const getLatest = () => axios.get(`${API_URL}/sensors/latest`);

export const getHistory = () =>
  axios.get(`${API_URL}/sensors/history`, {
    params: {
      from: "2024-01-01",
      to: new Date().toISOString(),
    },
  });

export const getLightStatus = () => axios.get(`${API_URL}/light/status`);

export const setLight = (state) =>
  axios.post(`${API_URL}/light?state=${state}`);
