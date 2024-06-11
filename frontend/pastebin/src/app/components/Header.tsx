import { AppBar, Button, IconButton, Toolbar, Typography } from "@mui/material";
import { IoIosMenu } from "react-icons/io";

type Props = {};

export const Header = (props: Props) => {
  return (
    <div>
      <AppBar
        className="pl-[14%] shadow-none"
        color="transparent"
        position="static"
      >
        <Toolbar variant="dense">
          <IconButton
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
          </IconButton>
          <Button href="/">
            <Typography variant="h6" color="inherit" component="div">
              PASTEBIN
            </Typography>
          </Button>
        </Toolbar>
      </AppBar>
    </div>
  );
};
