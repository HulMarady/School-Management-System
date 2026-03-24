const CreateUser = (function() {
    // Private variables
    let form = null;
    let passwordInput = null;
    let confirmPasswordInput = null;
    let confirmPasswordError = null;
    let submitButton = null;

    // Private methods
    function getElements() {
        form = document.getElementById('createUserForm');
        passwordInput = document.getElementById('Password');
        confirmPasswordInput = document.getElementById('ConfirmPassword');
        confirmPasswordError = document.getElementById('confirmPasswordError');
        submitButton = document.querySelector('button[type="submit"]');
    }

    function showError(element, message) {
        if (!element) return;
        
        // Remove existing error classes
        element.classList.remove('border-gray-300');
        element.classList.add('border-red-500', 'focus:border-red-500', 'focus:ring-red-500');
        
        // Show error message
        let errorSpan = element.parentElement?.parentElement?.querySelector('.text-red-600');
        if (!errorSpan) {
            errorSpan = element.closest('div')?.querySelector('.text-red-600');
        }
        if (errorSpan && errorSpan !== confirmPasswordError) {
            errorSpan.textContent = message;
        }
    }

    function clearError(element) {
        if (!element) return;
        
        // Remove error classes
        element.classList.remove('border-red-500', 'focus:border-red-500', 'focus:ring-red-500');
        element.classList.add('border-gray-300');
        
        // Hide error message
        let errorSpan = element.parentElement?.parentElement?.querySelector('.text-red-600');
        if (!errorSpan) {
            errorSpan = element.closest('div')?.querySelector('.text-red-600');
        }
        if (errorSpan && errorSpan !== confirmPasswordError && errorSpan.textContent) {
            errorSpan.textContent = '';
        }
    }

    function validatePasswordStrength(password) {
        const requirements = {
            length: password.length >= 8,
            uppercase: /[A-Z]/.test(password),
            lowercase: /[a-z]/.test(password),
            number: /[0-9]/.test(password),
            special: /[!@#$%^&*(),.?":{}|<>]/.test(password)
        };
        
        const isValid = Object.values(requirements).every(req => req === true);
        
        // Update requirement indicators if they exist
        const requirementsList = document.getElementById('passwordRequirements');
        if (requirementsList) {
            const lengthReq = requirementsList.querySelector('.req-length');
            const uppercaseReq = requirementsList.querySelector('.req-uppercase');
            const lowercaseReq = requirementsList.querySelector('.req-lowercase');
            const numberReq = requirementsList.querySelector('.req-number');
            const specialReq = requirementsList.querySelector('.req-special');
            
            if (lengthReq) {
                lengthReq.innerHTML = requirements.length ? 
                    '<i class="fas fa-check-circle text-green-500 text-xs"></i> At least 8 characters' : 
                    '<i class="fas fa-circle text-gray-300 text-xs"></i> At least 8 characters';
                lengthReq.className = requirements.length ? 'req-length text-green-600' : 'req-length text-gray-500';
            }
            
            if (uppercaseReq) {
                uppercaseReq.innerHTML = requirements.uppercase ? 
                    '<i class="fas fa-check-circle text-green-500 text-xs"></i> Contains uppercase letter' : 
                    '<i class="fas fa-circle text-gray-300 text-xs"></i> Contains uppercase letter';
                uppercaseReq.className = requirements.uppercase ? 'req-uppercase text-green-600' : 'req-uppercase text-gray-500';
            }
            
            if (lowercaseReq) {
                lowercaseReq.innerHTML = requirements.lowercase ? 
                    '<i class="fas fa-check-circle text-green-500 text-xs"></i> Contains lowercase letter' : 
                    '<i class="fas fa-circle text-gray-300 text-xs"></i> Contains lowercase letter';
                lowercaseReq.className = requirements.lowercase ? 'req-lowercase text-green-600' : 'req-lowercase text-gray-500';
            }
            
            if (numberReq) {
                numberReq.innerHTML = requirements.number ? 
                    '<i class="fas fa-check-circle text-green-500 text-xs"></i> Contains a number' : 
                    '<i class="fas fa-circle text-gray-300 text-xs"></i> Contains a number';
                numberReq.className = requirements.number ? 'req-number text-green-600' : 'req-number text-gray-500';
            }
            
            if (specialReq) {
                specialReq.innerHTML = requirements.special ? 
                    '<i class="fas fa-check-circle text-green-500 text-xs"></i> Contains special character' : 
                    '<i class="fas fa-circle text-gray-300 text-xs"></i> Contains special character';
                specialReq.className = requirements.special ? 'req-special text-green-600' : 'req-special text-gray-500';
            }
        }
        
        return isValid;
    }

    function validatePasswordMatch() {
        if (!passwordInput || !confirmPasswordInput) return true;
        
        const passwordsMatch = passwordInput.value === confirmPasswordInput.value;
        
        if (confirmPasswordError) {
            if (passwordInput.value && confirmPasswordInput.value && !passwordsMatch) {
                confirmPasswordError.textContent = 'Passwords do not match';
                confirmPasswordError.classList.remove('hidden');
                confirmPasswordInput.classList.add('border-red-500');
                confirmPasswordInput.classList.remove('border-gray-300');
                return false;
            } else {
                confirmPasswordError.textContent = '';
                confirmPasswordError.classList.add('hidden');
                confirmPasswordInput.classList.remove('border-red-500');
                confirmPasswordInput.classList.add('border-gray-300');
                return true;
            }
        }
        
        return passwordsMatch;
    }

    function validateEmail(email) {
        const emailRegex = /^[^\s@]+@([^\s@]+\.)+[^\s@]+$/;
        return emailRegex.test(email);
    }

    function validateUsername(username) {
        // Username can contain letters, numbers, and underscores
        const usernameRegex = /^[a-zA-Z0-9_]{3,50}$/;
        return usernameRegex.test(username);
    }

    function showSuccessMessage(message) {
        // Check if success message container exists
        let successContainer = document.getElementById('successMessage');
        
        if (!successContainer) {
            // Create success message container if it doesn't exist
            successContainer = document.createElement('div');
            successContainer.id = 'successMessage';
            successContainer.className = 'fixed top-4 right-4 bg-green-500 text-white px-6 py-3 rounded-lg shadow-lg z-50 transform transition-all duration-300 translate-x-full';
            document.body.appendChild(successContainer);
        }
        
        successContainer.innerHTML = `
            <div class="flex items-center gap-2">
                <i class="fas fa-check-circle"></i>
                <span>${message}</span>
            </div>
        `;
        
        // Show the message
        setTimeout(() => {
            successContainer.classList.remove('translate-x-full');
            successContainer.classList.add('translate-x-0');
        }, 100);
        
        // Hide after 3 seconds
        setTimeout(() => {
            successContainer.classList.remove('translate-x-0');
            successContainer.classList.add('translate-x-full');
            
            // Remove from DOM after animation
            setTimeout(() => {
                if (successContainer.parentNode) {
                    successContainer.parentNode.removeChild(successContainer);
                }
            }, 300);
        }, 3000);
    }

    function showErrorMessage(message) {
        // Check if error message container exists
        let errorContainer = document.getElementById('errorMessage');
        
        if (!errorContainer) {
            // Create error message container if it doesn't exist
            errorContainer = document.createElement('div');
            errorContainer.id = 'errorMessage';
            errorContainer.className = 'fixed top-4 right-4 bg-red-500 text-white px-6 py-3 rounded-lg shadow-lg z-50 transform transition-all duration-300 translate-x-full';
            document.body.appendChild(errorContainer);
        }
        
        errorContainer.innerHTML = `
            <div class="flex items-center gap-2">
                <i class="fas fa-exclamation-circle"></i>
                <span>${message}</span>
            </div>
        `;
        
        // Show the message
        setTimeout(() => {
            errorContainer.classList.remove('translate-x-full');
            errorContainer.classList.add('translate-x-0');
        }, 100);
        
        // Hide after 4 seconds
        setTimeout(() => {
            errorContainer.classList.remove('translate-x-0');
            errorContainer.classList.add('translate-x-full');
            
            // Remove from DOM after animation
            setTimeout(() => {
                if (errorContainer.parentNode) {
                    errorContainer.parentNode.removeChild(errorContainer);
                }
            }, 300);
        }, 4000);
    }

    function validateForm() {
        let isValid = true;
        const username = document.getElementById('Username')?.value;
        const email = document.getElementById('Email')?.value;
        const password = passwordInput?.value;
        const role = document.getElementById('Role')?.value;
        
        // Validate username
        if (username && !validateUsername(username)) {
            showError(document.getElementById('Username'), 'Username must be 3-50 characters and can only contain letters, numbers, and underscores');
            isValid = false;
        }
        
        // Validate email
        if (email && !validateEmail(email)) {
            showError(document.getElementById('Email'), 'Please enter a valid email address');
            isValid = false;
        }
        
        // Validate password
        if (password) {
            if (!validatePasswordStrength(password)) {
                showError(passwordInput, 'Password does not meet the requirements');
                isValid = false;
            }
        } else if (passwordInput) {
            showError(passwordInput, 'Password is required');
            isValid = false;
        }
        
        // Validate password match
        if (!validatePasswordMatch()) {
            isValid = false;
        }
        
        // Validate role
        if (!role) {
            showError(document.getElementById('Role'), 'Please select a role');
            isValid = false;
        }
        
        return isValid;
    }

    function setupEventListeners() {
        // Username validation
        const usernameInput = document.getElementById('Username');
        if (usernameInput) {
            usernameInput.addEventListener('blur', function() {
                if (this.value && !validateUsername(this.value)) {
                    showError(this, 'Username must be 3-50 characters and can only contain letters, numbers, and underscores');
                } else {
                    clearError(this);
                }
            });
            
            usernameInput.addEventListener('input', function() {
                clearError(this);
            });
        }
        
        // Email validation
        const emailInput = document.getElementById('Email');
        if (emailInput) {
            emailInput.addEventListener('blur', function() {
                if (this.value && !validateEmail(this.value)) {
                    showError(this, 'Please enter a valid email address');
                } else {
                    clearError(this);
                }
            });
            
            emailInput.addEventListener('input', function() {
                clearError(this);
            });
        }
        
        // Password validation
        if (passwordInput) {
            passwordInput.addEventListener('input', function() {
                if (this.value) {
                    validatePasswordStrength(this.value);
                    clearError(this);
                }
                validatePasswordMatch();
            });
        }
        
        // Confirm password validation
        if (confirmPasswordInput) {
            confirmPasswordInput.addEventListener('input', validatePasswordMatch);
        }
        
        // Role validation
        const roleSelect = document.getElementById('Role');
        if (roleSelect) {
            roleSelect.addEventListener('change', function() {
                if (this.value) {
                    clearError(this);
                }
            });
        }
        
        // Form submission
        if (form) {
            form.addEventListener('submit', function(e) {
                if (!validateForm()) {
                    e.preventDefault();
                    showErrorMessage('Please fix the errors before submitting');
                } else {
                    // Show loading state on submit button
                    if (submitButton) {
                        submitButton.disabled = true;
                        submitButton.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Creating...';
                    }
                }
            });
        }
    }

    function addPasswordRequirementsIndicator() {
        if (!passwordInput) return;
        
        // Check if requirements indicator already exists
        if (document.getElementById('passwordRequirements')) return;
        
        const passwordFieldContainer = passwordInput.closest('div');
        if (passwordFieldContainer && passwordFieldContainer.parentNode) {
            const parentDiv = passwordFieldContainer.parentNode;
            
            const requirementsHtml = `
                <div id="passwordRequirements" class="mt-2 text-xs space-y-1">
                    <p class="text-gray-600 mb-1"><i class="fas fa-shield-alt mr-1"></i> Password requirements:</p>
                    <ul class="space-y-1 ml-2">
                        <li class="req-length text-gray-500"><i class="fas fa-circle text-gray-300 text-xs"></i> At least 8 characters</li>
                        <li class="req-uppercase text-gray-500"><i class="fas fa-circle text-gray-300 text-xs"></i> Contains uppercase letter</li>
                        <li class="req-lowercase text-gray-500"><i class="fas fa-circle text-gray-300 text-xs"></i> Contains lowercase letter</li>
                        <li class="req-number text-gray-500"><i class="fas fa-circle text-gray-300 text-xs"></i> Contains a number</li>
                        <li class="req-special text-gray-500"><i class="fas fa-circle text-gray-300 text-xs"></i> Contains special character</li>
                    </ul>
                </div>
            `;
            
            parentDiv.insertAdjacentHTML('beforeend', requirementsHtml);
        }
    }

    // Public methods
    return {
        /**
         * Initialize the create user module
         */
        init: function() {
            getElements();
            addPasswordRequirementsIndicator();
            setupEventListeners();
            console.log('Create User module initialized');
        },
        
        /**
         * Validate form manually (can be called externally)
         */
        validate: function() {
            return validateForm();
        },
        
        /**
         * Show success message
         */
        showSuccess: function(message) {
            showSuccessMessage(message);
        },
        
        /**
         * Show error message
         */
        showError: function(message) {
            showErrorMessage(message);
        },
        
        /**
         * Reset form
         */
        resetForm: function() {
            if (form) {
                form.reset();
                // Clear any error states
                const errorFields = form.querySelectorAll('.border-red-500');
                errorFields.forEach(field => {
                    clearError(field);
                });
                
                // Reset password requirements
                const passwordField = document.getElementById('Password');
                if (passwordField) {
                    validatePasswordStrength('');
                }
                
                // Clear confirm password error
                if (confirmPasswordError) {
                    confirmPasswordError.textContent = '';
                    confirmPasswordError.classList.add('hidden');
                }
            }
        }
    };
})();

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        CreateUser.init();
    });
} else {
    CreateUser.init();
}

// Make CreateUser available globally
window.CreateUser = CreateUser;