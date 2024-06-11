import { Divider } from "@mui/material";
import React, { useEffect, useState } from "react";
import { IoEarthOutline } from "react-icons/io5";
import { api } from "../services/api";

type Props = {};

export const PastesList = (props: Props) => {
  const [pastes, setPastes] = useState<Paste[]>([]);
  
  const timeDifference = (date1: Date, date2: Date): string => {
    const diffInMs = Math.abs(date2.getTime() - date1.getTime());
    const diffInMinutes = Math.floor(diffInMs / (1000 * 60));
    const diffInHours = Math.floor(diffInMs / (1000 * 60 * 60));
    const diffInDays = Math.floor(diffInMs / (1000 * 60 * 60 * 24));

    if (diffInMinutes < 60) {
      return `${diffInMinutes} min ago`;
    } else if (diffInHours < 24) {
      return `${diffInHours} hour${diffInHours > 1 ? "s" : ""} ago`;
    } else {
      return `${diffInDays} day${diffInDays > 1 ? "s" : ""} ago`;
    }
  };

  useEffect(() => {
    api.getTopRecentPastes(10).then(res => {
      if (res.status == 200) {
        setPastes(res.data);
      }
    })
  }, []);
    
  return (
    <div>
      {pastes.map((e, i) => (
        <div key={i}>
          <Divider sx={{ borderStyle: "dotted" }} />
          <div className="m-[4px] flex">
            <IoEarthOutline className="mt-[5px] mr-[5px]"/>
            <div>
              <a href={e.id} className="text-blue-400">{e.title}</a>
              <p><small className="text-gray-500">{timeDifference(new Date(), new Date(e.createdAt))}</small></p>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};
