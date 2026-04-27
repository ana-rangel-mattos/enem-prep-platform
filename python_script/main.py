from psycopg2 import connect
from config import *
from constants import *
import requests
from time import sleep


def fetch_questions_by_year(year: int):
    limit = 45
    offset = 0

    while True:
        ENDPOINT = f"https://api.enem.dev/v1/exams/{year}/questions?limit={limit}&offset={offset}"

        response = requests.get(ENDPOINT)

        if response.status_code != 200:
            raise ConnectionError(
                f"Error connecting with the API.\n{response.json()}")

        data = response.json()

        has_more = data["metadata"]["hasMore"]

        if not has_more:
            break

        questions = data["questions"]
        sleep(1)

        offset += limit


def main():
    try:
        global connection
        connection = connect(
            host=HOST,
            database=DATABASE,
            user=USER,
            password=PASSWORD,
            port=PORT
        )

        cursor = connection.cursor()

        for year in range(FIRST_YEAR, LAST_YEAR):
            # TODO: INSERT ALL QUESTIONS FROM EACH YEAR
            fetch_questions_by_year(year)

        connection.commit()
        cursor.close()
    except Exception as e:
        print(f"Error: {e}")
    finally:
        if connection:
            connection.close()
            print("Database connection was closed!")


if __name__ == "__main__":
    main()
