"use client";

import { Api } from "@/app/services/api";
import { useEffect, useState } from "react";

export default function Page({ params }: { params: { id: string } }) {
  const [paste, setPaste] = useState<Paste>();
  const [error, setError] = useState<ApiError>();

  useEffect(() => {    
    Api.getPaste(params.id)
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
    <>
      <span>{paste?.text}</span>
    </>
  );
}
