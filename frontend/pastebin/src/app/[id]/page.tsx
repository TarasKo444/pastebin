"use client";

import { useEffect, useState } from "react";
import { MainLayout } from "../components/MainLayout";
import { api } from "../services/api";
import { TextArea } from "../components/TextArea";
import { Alert, Button } from "@mui/material";

export default function Page({ params }: { params: { id: string } }) {
  const [paste, setPaste] = useState<Paste>();
  const [error, setError] = useState<ApiError>();

  useEffect(() => {
    api.getPaste(params.id)
      .then((res) => {
        if (res.status == 200) {
          setPaste(res.data);
        }
      })
      .catch(({ response }: { response: { data: ApiError } }) => {
        setError(response.data);
      });
  }, [params.id]);

  return (
    <MainLayout>
      <div className="w-[75%]">
        <div className="p-3 *:mt-4">
          {error && <Alert severity="error">{error.errors[0]}</Alert>}
          {paste && (
            <>
              <div className="flex justify-between">
                <h1>{paste.title}</h1>
                <Button href={`raw/${paste.id}`} className="" variant="contained">Raw</Button>
              </div>
              <TextArea
                readonly={true}
                value={paste.text}
                className="overflow-auto whitespace-pre resize-none h-[500px]"
              />
            </>
          )}
        </div>
      </div>
    </MainLayout>
  );
}
