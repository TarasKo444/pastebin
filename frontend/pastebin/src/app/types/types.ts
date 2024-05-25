interface Paste {
  id: string;
  text: string;
  title: string;
  expirationTime: string | null;
  createdAt: string;
}

interface PastePostDto {
  text: string;
  title: string;
  expirationTime: string | null;
}

interface ApiError {
  status: number;
  errors: string[];
}