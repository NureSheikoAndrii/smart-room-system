import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
} from "recharts";

// Функція форматування дати
const formatDate = (value) => {
  const date = new Date(value);
  return date.toLocaleString("uk-UA", {
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
  });
};

export default function SensorChart({ data }) {
  return (
    <LineChart width={700} height={300} data={data}>
      <CartesianGrid strokeDasharray="3 3" />

      {/* Читабельний час замість ISO */}
      <XAxis dataKey="createdAt" tickFormatter={formatDate} />

      <YAxis />
      <Tooltip labelFormatter={formatDate} />

      <Line type="monotone" dataKey="temperature" name="Температура (°C)" />
      <Line type="monotone" dataKey="humidity" name="Вологість (%)" />
    </LineChart>
  );
}
