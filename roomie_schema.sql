CREATE TABLE IF NOT EXISTS rooms (
  id SERIAL PRIMARY KEY,
  title TEXT NOT NULL,
  city  TEXT NOT NULL,
  price INT  NOT NULL CHECK (price > 0),
  description TEXT,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

INSERT INTO rooms (title, city, price, description) VALUES
('Lyst værelse på Nørrebro', 'Copenhagen', 5500, 'Tæt på metro.'),
('Rum i Aarhus C', 'Aarhus', 4300, 'Roligt kollektiv.')
ON CONFLICT DO NOTHING;