"use client";

import { TextArea } from "@/app/components/TextArea";
import {
  Button,
  Divider,
  TextField,
  ThemeProvider,
  createTheme,
} from "@mui/material";
import { SelectExpiration } from "@/app/components/Select";
import { PastesList } from "./components/PastesList";
import { PiUserCircleFill } from "react-icons/pi";
import { Header } from "./components/Header";

const darkTheme = createTheme({
  palette: {
    mode: "dark",
  },
});

export default function Home() {
  return (
    <main className="bg-[#202324] text-white">
      <ThemeProvider theme={darkTheme}>
        <Header />
        <div className="h-[150vh] flex justify-center items-center flex-col">
          <div className="h-full flex flex-row border-gray-500 border w-[70%] bg-[#1c1e1f]">
            <div className="w-[75%]">
              <div className="p-3 *:mt-4">
                <h1>Create Paste</h1>
                <TextArea className="overflow-auto whitespace-pre resize-none h-[500px]" />
                <Divider textAlign="left">Optional</Divider>
                <div className="flex">
                  <div className="*:mt-4 w-[50%]">
                    <TextField
                      id="outlined-basic"
                      label="Title"
                      variant="outlined"
                    />
                    <SelectExpiration className="w-56" />
                    <Button color="success" variant="contained">Create</Button>
                  </div>
                  <div className="w-[50%]">
                    <div className="flex w-[300px]">
                      <PiUserCircleFill className="w-auto h-[100px]" />
                      <div className="*:mt-3">
                        <p>Hello Guest</p>
                        <div className="flex *:p-1">
                          <Button variant="contained">Register</Button>
                          <p>or</p>
                          <Button variant="contained">Login</Button>
                        </div>
                      </div>
                    </div>
                    <Button
                      color="error"
                      variant="contained"
                      className="w-[300px]"
                    >
                      Sign in with Google
                    </Button>
                  </div>
                </div>
              </div>
            </div>
            <div className="w-[25%]">
              <div className="p-3 *:mt-4">
                <h1>Public Pastes</h1>
                <PastesList></PastesList>
              </div>
            </div>
          </div>
        </div>
      </ThemeProvider>
    </main>
  );
}
