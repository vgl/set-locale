Feature: User Management
  Scenario: New User


    When user views "/user/new" 
            And must fill "User Name" field and "E-Mail" and "Password"
            And clicks "Save"
            And "User Name" or "E-mail" or "Password" is not already added


    Then data should be saved

