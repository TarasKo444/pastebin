"use client";

import { TextArea } from "@/app/components/TextArea";
import { Button, Divider, TextField } from "@mui/material";
import { SelectExpiration } from "@/app/components/Select";
import { PastesList } from "./components/PastesList";
import { PiUserCircleFill } from "react-icons/pi";
import { MainLayout } from "./components/MainLayout";
import { FormEvent } from "react";
import { api } from "./services/api";
import { useRouter } from "next/navigation";

export default function Home() {
  const router = useRouter();

  async function onSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const object: { [key: string]: any } = {};
    formData.forEach((value, key) => (object[key] = value));
    var txt = JSON.stringify(object);
    var dto: PastePostDto = JSON.parse(txt);
    dto.expirationTime = dto.expirationTime == "" ? null : dto.expirationTime;

    console.log(dto);

    await api
      .createPaste(dto)
      .then((res) => {
        if (res.status == 200) {
          console.log(res.data);
          router.push(`/${res.data.id}`);
        }
      })
      .catch(console.log);
  }

  return (
    <MainLayout>
      <div className="w-[75%]">
        <form className="p-3 *:mt-4" onSubmit={onSubmit}>
          <h1>Create Paste</h1>
          <TextArea
            name="text"
            className="overflow-auto whitespace-pre resize-none h-[500px]"
          />
          <Divider textAlign="left">Optional</Divider>
          <div className="flex">
            <div className="*:mt-4 w-[50%]">
              <TextField
                type="text"
                name="title"
                id="outlined-basic"
                label="Title"
                variant="outlined"
              />
              <SelectExpiration name="expirationTime" className="w-56" />
              <Button type="submit" color="success" variant="contained">
                Create
              </Button>
            </div>
            <div className="w-[50%]">
              <div className="flex w-[300px]">
                <PiUserCircleFill className="w-auto h-[100px]" />
                <div className="*:mt-3">
                  <p>Hello Guest</p>
                  <Button color="error" variant="contained">
                    Sign in with Google
                  </Button>
                </div>
              </div>
            </div>
          </div>
        </form>
      </div>
      <div className="w-[25%]">
        <div className="p-3 *:mt-4">
          <h1>Public Pastes</h1>
          <PastesList></PastesList>
        </div>
      </div>
    </MainLayout>
  );
}
