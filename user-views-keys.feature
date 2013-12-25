Feature: My Keys

    Scenario: User views his/her keys

    Given user should be logged in
        And user is in "Developer" or "Translator" role
        And user views home "/"

    When user clicks "Kelimelerim" button

    Then system lists users related keys table with these columns "Edit" button, Keys, Tag, Translated Languages 
        And this list is ordered descending by updatedAt time.
        
