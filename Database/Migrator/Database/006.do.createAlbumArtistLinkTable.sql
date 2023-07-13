CREATE TABLE IF NOT EXISTS album_artist_links (
    id uuid PRIMARY KEY NOT NULL DEFAULT uuid_generate_v4(),
    album_id uuid NOT NULL,  
    artist_id uuid NOT NULL,
    created_at timestamp(3) DEFAULT NOW(),
    updated_at timestamp(3)
);

CREATE INDEX IF NOT EXISTS album_artist_links_album_id
ON album_artist_links(album_id);

CREATE INDEX IF NOT EXISTS album_artist_links_artist_id
ON album_artist_links(artist_id);

ALTER TABLE album_artist_links
      DROP CONSTRAINT IF EXISTS fk_album_artist_links_album_id;  
          
ALTER TABLE album_artist_links
      ADD CONSTRAINT fk_album_artist_links_album_id FOREIGN KEY (album_id)
          REFERENCES albums (id)
          ON DELETE SET NULL;

ALTER TABLE album_artist_links
      DROP CONSTRAINT IF EXISTS fk_album_artist_links_artist_id;  
          
ALTER TABLE album_artist_links
      ADD CONSTRAINT fk_album_artist_links_artist_id FOREIGN KEY (artist_id)
          REFERENCES artists (id)
          ON DELETE SET NULL;


ALTER TABLE album_artist_links DROP CONSTRAINT IF EXISTS album_artist_unique;

ALTER TABLE album_artist_links ADD CONSTRAINT album_artist_unique UNIQUE (album_id,artist_id);