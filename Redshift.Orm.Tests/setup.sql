CREATE ROLE redshifttest WITH LOGIN PASSWORD '1234';
ALTER DATABASE redshifttest OWNER TO redshifttest;

GRANT ALL PRIVILEGES ON DATABASE redshifttest TO redshifttest;

\connect redshifttest

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: public; Type: SCHEMA; Schema: -; Owner: redshifttest
--

CREATE SCHEMA IF NOT EXISTS public;

ALTER SCHEMA public OWNER TO redshifttest;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: redshifttest
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


--
-- Name: public; Type: ACL; Schema: -; Owner: redshifttest
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM redshifttest;
GRANT ALL ON SCHEMA public TO redshifttest;
GRANT ALL ON SCHEMA public TO PUBLIC;