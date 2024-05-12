import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";

export const SelectExpiration = ({ className }: { className?: string }) => {
  return (
    <>
      <FormControl className={`${className ?? 'min-w-56'}`}>
        <InputLabel id="demo-simple-select-label">Expiration time</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          label="Expiration time"
        >
          <MenuItem value={10}>None</MenuItem>
          <MenuItem value={20}>1 Minute</MenuItem>
          <MenuItem value={30}>10 Minutes</MenuItem>
          <MenuItem value={40}>1 Hour</MenuItem>
          <MenuItem value={50}>6 Hours</MenuItem>
          <MenuItem value={60}>12 Hours</MenuItem>
          <MenuItem value={70}>1 Day</MenuItem>
          <MenuItem value={80}>1 Week</MenuItem>
          <MenuItem value={90}>1 Month</MenuItem>
          <MenuItem value={100}>6 Months</MenuItem>
          <MenuItem value={110}>1 Year</MenuItem>
        </Select>
      </FormControl>
    </>
  );
};
