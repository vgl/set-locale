Feature: User Management
  Scenario: Reset Password 


    When user views "/user/newpassword" 
            And must fill "E-Mail"
            And clicks "Send Reset Password Link" button
            And checks if this e-mail has an account


    Then new password link should be send to this e-mail