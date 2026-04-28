from psycopg2 import connect
from config import *
from constants import *
import requests
import json
from time import sleep


def map_discipline_to_id(discipline: str) -> str:
    match discipline:
        case "linguagens":
            return LANGUAGES_ID
        case "ciencias-humanas":
            return SOCIAL_SCIENCES_ID
        case "ciencias-natureza":
            return NATURAL_SCIENCES_ID
        case "matematica":
            return MATH_ID

def insert_into_database(cursor, questions: list[dict]) -> None:
    rows = []
    for question in questions:
        discipline_id = map_discipline_to_id(question["discipline"])
        question_json = json.dumps(question)

        lang = question.get("language")
        if lang:
            lang = lang.upper()

        data = (
                discipline_id, 
                POSTED_BY_ID, 
                question.get("index"), 
                lang, 
                question_json
        )
        rows.append(data)

    cursor.executemany(QUESTION_INSERTION_QUERY, rows)

def fetch_questions_by_year(year: int):
    """Return all questions from `year`.

    Attributes:
        year (int): The exam year.

    Return:
        list: A list with all questions in a dictionary format.    
    """
    limit = 45
    offset = 0
    final_data = []

    while True:
        ENDPOINT = f"https://api.enem.dev/v1/exams/{year}/questions?limit={limit}&offset={offset}"

        response = requests.get(ENDPOINT)

        if response.status_code != 200:
            raise ConnectionError(
                f"Error connecting with the API.\n{response.json()}")

        data = response.json()
        questions = data["questions"]
        has_more = data["metadata"]["hasMore"]
        final_data.extend(questions)

        if not has_more:
            break

        sleep(1)
        offset += limit
    
    return final_data


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
            questions = fetch_questions_by_year(year)
            insert_into_database(cursor, questions)

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
