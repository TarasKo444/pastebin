import { ThemeProvider, createTheme } from "@mui/material"
import { Header } from "./Header"
import { ReactNode } from "react";

const darkTheme = createTheme({
  palette: {
    mode: "dark",
  },
});  

type Props = {
  children?: ReactNode;
};

export const MainLayout = ({ children }: Props) => {
  return (
    <main className="bg-[#202324] text-white">
      <ThemeProvider theme={darkTheme}>
        <Header />
        <div className="min-h-[150vh] flex items-center flex-col">
          <div className="min-h-[150vh] p-2 h-full flex flex-row border-gray-500 border w-[70%] bg-[#1c1e1f]">
            {children}
          </div>
        </div>
      </ThemeProvider>
    </main>
  );
};