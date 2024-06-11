import React, { useState } from 'react';
import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import moment from 'moment';

export const SelectExpiration = ({
  className,
  name,
}: {
  className?: string;
  name?: string;
}) => {
  const [expirationValue, setExpirationValue] = useState(0);

  const handleExpirationChange = (event: any) => {
    const value = event.target.value;
    setExpirationValue(value);
  };

  const calculateExpirationDate = (value: number) => {
    let date: Date | null = new Date();
    switch (value) {
      case 1:
        date.setMinutes(date.getMinutes() + 1);
        break;
      case 2:
        date.setMinutes(date.getMinutes() + 10);
        break;
      case 3:
        date.setHours(date.getHours() + 1);
        break;
      case 4:
        date.setHours(date.getHours() + 6);
        break;
      case 5:
        date.setHours(date.getHours() + 12);
        break;
      case 6:
        date.setDate(date.getDate() + 1);
        break;
      case 7:
        date.setDate(date.getDate() + 7);
        break;
      case 8:
        date.setMonth(date.getMonth() + 1);
        break;
      case 9:
        date.setMonth(date.getMonth() + 6);
        break;
      case 10:
        date.setFullYear(date.getFullYear() + 1);
        break;
      default:
        date = null;
        break;
    }
    
    return date ? moment(date).format().replace('T', ' ').replace('+', ' +') : ""
  };

  return (
    <div>
      <FormControl className={`${className ?? "min-w-56"}`}>
        <InputLabel id="demo-simple-select-label">Expiration time</InputLabel>
        <input
          type="hidden"
          value={calculateExpirationDate(expirationValue)}
          name={name}
        ></input>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={expirationValue}
          label="Expiration time"
          onChange={handleExpirationChange}
        >
          <MenuItem value={0}>None</MenuItem>
          <MenuItem value={1}>1 Minute</MenuItem>
          <MenuItem value={2}>10 Minutes</MenuItem>
          <MenuItem value={3}>1 Hour</MenuItem>
          <MenuItem value={4}>6 Hours</MenuItem>
          <MenuItem value={5}>12 Hours</MenuItem>
          <MenuItem value={6}>1 Day</MenuItem>
          <MenuItem value={7}>1 Week</MenuItem>
          <MenuItem value={8}>1 Month</MenuItem>
          <MenuItem value={9}>6 Months</MenuItem>
          <MenuItem value={10}>1 Year</MenuItem>
        </Select>
      </FormControl>
    </div>
  );
};
