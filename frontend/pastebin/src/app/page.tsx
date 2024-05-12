"use client";

import { Divider, TextField } from "@mui/material";
import { TextArea } from "./components/TextArea";
import { SelectExpiration } from "./components/Select";

export default function Home() {  
  return (
    <main className="h-[150vh] flex justify-center items-center flex-col bg-black text-black">
      <div className="h-full flex flex-col border-gray-500 border-r border-l w-[50%] bg-white">
        <div className="p-3 *:mt-4">
          <h1>Create Paste</h1>
          <TextArea className="overflow-auto whitespace-pre resize-none h-[500px]"/>
          <TextField id="outlined-basic" label="Title" variant="outlined" />
          <Divider>Optional</Divider>
          <SelectExpiration className="w-56"/>
        </div>
      </div>
    </main>
  );
}
