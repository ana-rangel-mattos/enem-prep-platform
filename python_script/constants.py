FIRST_YEAR = 2009
LAST_YEAR = 2023


QUESTION_INSERTION_QUERY = """
INSERT INTO content.question (posted_by_id, subject_id, api_index, language, content)
VALUES (%s, %s, %s, %s, %s);"""
