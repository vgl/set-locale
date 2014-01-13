Feature: User Management
  Scenario: Reset Password 

    Given a visitor views "/user/passwordreset"
    
    When clicks "Send Reset Password Link" button
            And visitor must fill "email"
            And a link is not already send in an hour
            And system warns visitor to signup if this "email" has no account

    Then new password link should be send to this "eMail"
