import { useEffect, useState } from "react";
import { Container, Typography, Grid, Box, CssBaseline } from "@mui/material";

import { getLatest, getHistory, getLightStatus, setLight } from "./api/api";

import SensorCard from "./components/SensorCard";
import SensorChart from "./components/SensorChart";
import LightControl from "./components/LightControl";

export default function App() {
  const [latest, setLatest] = useState(null);
  const [history, setHistory] = useState([]);
  const [light, setLightState] = useState(false);

  useEffect(() => {
    loadData();
    const interval = setInterval(loadData, 10000);
    return () => clearInterval(interval);
  }, []);

  const loadData = async () => {
    try {
      const latestRes = await getLatest();
      const historyRes = await getHistory();
      const lightRes = await getLightStatus();

      setLatest(latestRes.data);
      setHistory(historyRes.data);
      setLightState(lightRes.data.light);
    } catch (e) {
      console.error(e);
    }
  };

  const toggleLight = async () => {
    await setLight(!light);
    setLightState(!light);
  };

  if (!latest) {
    return (
      <Container sx={{ mt: 10 }}>
        <Typography variant="h6">Завантаження...</Typography>
      </Container>
    );
  }

  return (
    <>
      <CssBaseline />

      {/* 🌍 ФОН УСІЄЇ СТОРІНКИ */}
      <div
        style={{
          minHeight: "100vh",
          backgroundColor: light ? "#9e9e9e" : "#000000",
          transition: "background-color 0.4s ease",
        }}
      >
        <Container sx={{ mt: 4, color: "#ffffff" }}>
          <Typography variant="h4" gutterBottom>
            Smart Room Dashboard
          </Typography>

          <Grid container spacing={3}>
            <Grid item xs={12} sm={4}>
              <SensorCard
                title="Температура"
                value={`${latest.temperature} °C`}
              />
            </Grid>

            <Grid item xs={12} sm={4}>
              <SensorCard title="Вологість" value={`${latest.humidity} %`} />
            </Grid>

            <Grid item xs={12} sm={4}>
              <SensorCard
                title="Освітленість"
                value={`${latest.lightLevel} lux`}
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 5 }}>
            <Typography variant="h6" gutterBottom>
              Історія
            </Typography>
            <SensorChart data={history} />
          </Box>

          <Box sx={{ mt: 4 }}>
            <LightControl isOn={light} onToggle={toggleLight} />
          </Box>
        </Container>
      </div>
    </>
  );
}
