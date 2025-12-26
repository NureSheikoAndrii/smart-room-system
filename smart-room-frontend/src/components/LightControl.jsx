import { Card, CardContent, Typography, Button } from "@mui/material";
import LightbulbIcon from "@mui/icons-material/Lightbulb";

export default function LightControl({ isOn, onToggle }) {
  return (
    <Card elevation={4} sx={{ maxWidth: 300 }}>
      <CardContent>
        <Typography variant="h6">Світло</Typography>

        <Typography sx={{ mt: 1 }}>
          Стан: {isOn ? "Увімкнено" : "Вимкнено"}
        </Typography>

        <Button
          variant="contained"
          color={isOn ? "error" : "success"}
          startIcon={<LightbulbIcon />}
          sx={{ mt: 2 }}
          onClick={onToggle}
          fullWidth
        >
          {isOn ? "Вимкнути" : "Увімкнути"}
        </Button>
      </CardContent>
    </Card>
  );
}
