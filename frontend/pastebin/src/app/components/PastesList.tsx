import { Divider } from "@mui/material";
import React from "react";
import { IoEarthOutline } from "react-icons/io5";

type Props = {};

export const PastesList = (props: Props) => {
  let list: Paste[] = [
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
    {
      id: "adasdsd",
      text: "test",
      title: "title",
      expirationTime: "123",
      createdAt: "10 min age",
    },
  ];

  return (
    <div>
      {list.map((e, i) => (
        <div key={i}>
          <Divider sx={{ borderStyle: "dotted" }} />
          <div className="m-[4px] flex">
            <IoEarthOutline className="mt-[5px] mr-[5px]"/>
            <div>
              <a href="#" className="text-blue-400">{e.title}</a>
              <p><small className="text-gray-500">{e.createdAt}</small></p>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};
