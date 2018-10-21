CREATE TABLE IF NOT EXISTS users (
    uname VARCHAR(30) UNIQUE NOT NULL,
    pw CHAR(64) NOT NULL,
    persist BOOLEAN,
    machineName VARCHAR(300),
    PRIMARY KEY (uname)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS automata (
	aid INT AUTO_INCREMENT,
    atitle VARCHAR(30),
    adesc VARCHAR(300),
    enabled BOOLEAN,
    asettings TEXT,
    PRIMARY KEY (aid)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS runs (
	rid INT,
    aid INT,
    rtime FLOAT,
    rdate DATETIME,
    rresult TINYINT,
    PRIMARY KEY (rid, aid),
    FOREIGN KEY (aid)
		REFERENCES automata(aid)
        ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS steps (
	sid INT AUTO_INCREMENT,
    scategory TINYINT,
    sname VARCHAR(30),
    sdesc VARCHAR(300),
    istrigger BOOLEAN,
    PRIMARY KEY (sid)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS userautomaton (
	uname VARCHAR(30),
    aid INT,
    role TINYINT,
    PRIMARY KEY (uname, aid),
    FOREIGN KEY (uname)
		REFERENCES users(uname)
        ON DELETE CASCADE,
	FOREIGN KEY (aid)
		REFERENCES automata(aid)
        ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS automatastep (
	asid INT,
    aid INT,
    sid INT,
    astitle VARCHAR(30),
    asdesc VARCHAR(300),
    parentid INT,
    runafter TINYINT,
    assettings TEXT,
    PRIMARY KEY (asid, aid, sid),
    FOREIGN KEY (parentid)
		REFERENCES automatastep(asid)
        ON DELETE CASCADE,
	FOREIGN KEY (aid)
		REFERENCES automata(aid)
        ON DELETE CASCADE,
	FOREIGN KEY (sid)
		REFERENCES steps(sid)
        ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS runstep (
	rsid INT,
    rid INT,
    rstitle VARCHAR(30),
    rsdesc VARCHAR(300),
    rsresult TINYINT,
    rsoutput TEXT,
    PRIMARY KEY (rsid, rid),
    FOREIGN KEY (rid)
		REFERENCES runs(rid)
        ON DELETE CASCADE
) ENGINE=InnoDB;