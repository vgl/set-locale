Feature: User Management
  Scenario: Visitor wants to sign up
    Given a visitor views "/user/login"
    When clicks "Sign up"  button
    Then system reditects to "/user/new"

  Scenario: Users wants to reset password
    Given a user views "/user/login"
    When clicks "Forget your password?"  button
    Then system reditects to "/user/reset"

  Scenario: Users wants to login
    Given a user views "/user/login"  

    When fills "E-Mail" and "Password" fields
    And clicks "Sign up" button
    And "Email" is valid email
    And user has an account with this "E-mail"
    And email, password authenticates this user    

    Then user should be directed to the main page