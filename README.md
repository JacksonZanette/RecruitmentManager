# RecruitmentManager
## User Story

    Given that as a technical evaluator from the company I want to store candidates scores for the technical assessment
    Then I want a simple system to store the name and the score of the candidates.

## Acceptance criteria
    1. The system should allow the insertion of candidates with or without the score;
    2. The system should validate that the candidate name is provided and not empty;
    3. The system should validate that the score should be between 0-100 if provided;
    4. The system should allow the deletion of candidates;
    5. The system should allow the update of candidates' names and scores;
    6. The system should allow the retrieval of specific candidates by their ids;
    7. The system should allow the creation of users to interact with;
    8. All actions except the user creation and login should be authorized;
    9. The system should validate that the user name and password are provided and not empty;
    10. The system should not allow the creation of duplicated users by their names;
    11. The system should allow to retrieve all the users names, once the current user is logged in, ommiting their passwords;
