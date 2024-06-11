"use client";

import { api } from "@/app/services/api";
import { Alert } from "@mui/material";
import { useEffect, useState } from "react";

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

  console.log(paste?.text);
  
  
  return (
    <>
      {error && <Alert severity="error">{error.errors[0]}</Alert>}
      <pre>{paste?.text}</pre>
    </>
  );
}
