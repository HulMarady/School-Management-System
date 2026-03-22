// Toggle password visibility for all toggle buttons
document.querySelectorAll('.toggle-password').forEach(button => {
    button.addEventListener('click', function() {
        const targetId = this.getAttribute('data-target');
        const input = document.getElementById(targetId);
        const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
        input.setAttribute('type', type);
        
        // Toggle icon
        const svg = this.querySelector('svg');
        if (type === 'text') {
            svg.innerHTML = '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />';
        } else {
            svg.innerHTML = '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />';
        }
    });
});

// Helper functions for error display
function showError(elementId, message) {
    const errorElement = document.getElementById(elementId);
    errorElement.textContent = message;
    errorElement.classList.remove('hidden');
    
    // Add red border to input
    const input = errorElement.previousElementSibling;
    if (input && input.tagName === 'INPUT') {
        input.classList.add('border-red-500', 'focus:ring-red-500', 'focus:border-red-500');
    }
}

function hideError(elementId) {
    const errorElement = document.getElementById(elementId);
    errorElement.classList.add('hidden');
    
    // Remove red border from input
    const input = errorElement.previousElementSibling;
    if (input && input.tagName === 'INPUT') {
        input.classList.remove('border-red-500', 'focus:ring-red-500', 'focus:border-red-500');
    }
}

// Validation functions
function validateUsername() {
    const username = document.getElementById('Username').value.trim();
    if (username.length < 3) {
        showError('usernameError', 'Username must be at least 3 characters');
        return false;
    } else if (!/^[a-zA-Z0-9_]+$/.test(username)) {
        showError('usernameError', 'Username can only contain letters, numbers, and underscores');
        return false;
    }
    hideError('usernameError');
    return true;
}

function validateEmail() {
    const email = document.getElementById('Email').value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        showError('emailError', 'Please enter a valid email address');
        return false;
    }
    hideError('emailError');
    return true;
}

function validatePassword() {
    const password = document.getElementById('Password').value;
    if (password.length < 8) {
        showError('passwordError', 'Password must be at least 8 characters');
        return false;
    }
    hideError('passwordError');
    return true;
}

function validateConfirmPassword() {
    const password = document.getElementById('Password').value;
    const confirmPassword = document.getElementById('ConfirmPassword').value;
    if (password !== confirmPassword) {
        showError('confirmPasswordError', 'Passwords do not match');
        return false;
    }
    hideError('confirmPasswordError');
    return true;
}

// Real-time validation event listeners
document.addEventListener('DOMContentLoaded', function() {
    const usernameInput = document.getElementById('Username');
    const emailInput = document.getElementById('Email');
    const passwordInput = document.getElementById('Password');
    const confirmPasswordInput = document.getElementById('ConfirmPassword');
    const form = document.getElementById('registerForm');
    
    if (usernameInput) {
        usernameInput.addEventListener('input', validateUsername);
    }
    
    if (emailInput) {
        emailInput.addEventListener('input', validateEmail);
    }
    
    if (passwordInput) {
        passwordInput.addEventListener('input', function() {
            validatePassword();
            if (confirmPasswordInput && confirmPasswordInput.value) {
                validateConfirmPassword();
            }
        });
    }
    
    if (confirmPasswordInput) {
        confirmPasswordInput.addEventListener('input', validateConfirmPassword);
    }
    
    // Form submit validation
    if (form) {
        form.addEventListener('submit', function(e) {
            const isUsernameValid = validateUsername();
            const isEmailValid = validateEmail();
            const isPasswordValid = validatePassword();
            const isConfirmPasswordValid = validateConfirmPassword();
            
            if (!isUsernameValid || !isEmailValid || !isPasswordValid || !isConfirmPasswordValid) {
                e.preventDefault();
                
                // Scroll to first error
                const firstError = document.querySelector('.error-message:not(.hidden)');
                if (firstError) {
                    firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                }
            }
        });
    }
});