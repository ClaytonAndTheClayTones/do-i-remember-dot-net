    CREATE TABLE IF NOT EXISTS labels (
        id UUID PRIMARY KEY, 
        name TEXT NOT NULL,
        city TEXT,
        state TEXT
    );
