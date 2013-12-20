Feature: User Management
  Scenario: New User

  	Given a visitor

    When views "/user/new" 
    And fills required "User Name", "E-mail" and "Password" fields
    And "E-mail" should be valid email
    And "E-mail" is not already added
    And clicks "Save" button

