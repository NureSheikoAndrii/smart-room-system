import { Card, CardContent, Typography } from "@mui/material";

export default function SensorCard({ title, value }) {
  return (
    <Card elevation={4}>
      <CardContent>
        <Typography variant="h6">{title}</Typography>
        <Typography variant="h4">{value}</Typography>
      </CardContent>
    </Card>
  );
}
