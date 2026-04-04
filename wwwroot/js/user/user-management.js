const UserManagement = (function() {
    // Private variables
    let modalElement = null;
    let modalTitle = null;
    let modalContent = null;
    let modalConfirmBtn = null;

    // Private methods
    function getModalElements() {
        modalElement = document.getElementById('userModal');
        modalTitle = document.getElementById('modalTitle');
        modalContent = document.getElementById('modalContent');
        modalConfirmBtn = document.getElementById('modalConfirmBtn');
    }

    function openModal() {
        if (modalElement) {
            modalElement.classList.remove('hidden');
            modalElement.classList.add('flex');
            document.body.style.overflow = 'hidden';
        }
    }

    function closeModal() {
        if (modalElement) {
            modalElement.classList.add('hidden');
            modalElement.classList.remove('flex');
            document.body.style.overflow = '';
        }
    }

    function escapeHtml(text) {
        if (!text) return '';
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    function getRoleStyles(role) {
        const styles = {
            'Admin': {
                class: 'bg-purple-100 text-purple-800',
                icon: 'fa-user-shield'
            },
            'Teacher': {
                class: 'bg-blue-100 text-blue-800',
                icon: 'fa-chalkboard-user'
            },
            'Student': {
                class: 'bg-green-100 text-green-800',
                icon: 'fa-graduation-cap'
            }
        };
        return styles[role] || {
            class: 'bg-gray-100 text-gray-800',
            icon: 'fa-user'
        };
    }

    function formatDate(dateString) {
        if (!dateString) return 'N/A';
        const date = new Date(dateString);
        return date.toLocaleString();
    }

    // Public methods
    return {
        /**
         * Initialize the user management module
         */
        init: function() {
            getModalElements();
            
            // Setup event listeners for modal
            if (modalElement) {
                // Close modal when clicking outside
                modalElement.addEventListener('click', function(e) {
                    if (e.target === modalElement) {
                        closeModal();
                    }
                });
                
                // Close modal on escape key
                document.addEventListener('keydown', function(e) {
                    if (e.key === 'Escape') {
                        closeModal();
                    }
                });
            }
            
            console.log('User Management module initialized');
        },

        /**
         * View user details
         * @param {number} userId - The user ID
         */
        viewUser: function(userId) {
            fetch(`/User/GetUserDetails/${userId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    if (modalTitle) modalTitle.innerText = 'User Details';
                    if (modalContent) {
                        const roleStyle = getRoleStyles(data.role);
                        modalContent.innerHTML = `
                            <div class="space-y-4">
                                <div class="flex items-center gap-3 p-4 bg-gray-50 rounded-lg">
                                    <div class="w-14 h-14 rounded-full bg-gradient-to-r from-indigo-500 to-indigo-600 flex items-center justify-center text-white text-xl font-bold">
                                        ${escapeHtml(data.username?.charAt(0).toUpperCase() || 'U')}
                                    </div>
                                    <div class="flex-1">
                                        <h4 class="font-semibold text-gray-800 text-lg">${escapeHtml(data.username)}</h4>
                                        <p class="text-sm text-gray-500">ID: #${data.id}</p>
                                    </div>
                                </div>
                                
                                <div class="grid grid-cols-2 gap-3 text-sm">
                                    <div class="p-3 bg-gray-50 rounded-lg">
                                        <p class="text-gray-500 text-xs mb-1">Email Address</p>
                                        <p class="font-medium text-gray-800 break-all">${escapeHtml(data.email)}</p>
                                    </div>
                                    <div class="p-3 bg-gray-50 rounded-lg">
                                        <p class="text-gray-500 text-xs mb-1">Role</p>
                                        <p class="font-medium">
                                            <span class="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs ${roleStyle.class}">
                                                <i class="fas ${roleStyle.icon} text-xs"></i>
                                                ${escapeHtml(data.role || 'Not Assigned')}
                                            </span>
                                        </p>
                                    </div>
                                    <div class="p-3 bg-gray-50 rounded-lg">
                                        <p class="text-gray-500 text-xs mb-1">Created At</p>
                                        <p class="font-medium text-gray-800">${formatDate(data.createdAt)}</p>
                                    </div>
                                    <div class="p-3 bg-gray-50 rounded-lg">
                                        <p class="text-gray-500 text-xs mb-1">Last Updated</p>
                                        <p class="font-medium text-gray-800">${formatDate(data.updatedAt)}</p>
                                    </div>
                                </div>
                                
                                <div class="mt-3 p-3 bg-yellow-50 rounded-lg border border-yellow-200">
                                    <p class="text-xs text-yellow-700 flex items-center gap-1">
                                        <i class="fas fa-lock"></i>
                                        <span>Password is encrypted and cannot be viewed</span>
                                    </p>
                                </div>
                            </div>
                        `;
                    }
                    
                    if (modalConfirmBtn) modalConfirmBtn.classList.add('hidden');
                    openModal();
                })
                .catch(error => {
                    console.error('Error viewing user:', error);
                    alert('Failed to load user details. Please try again.');
                });
        },

        /**
         * Edit user - redirect to edit page
         * @param {number} userId - The user ID
         */
        editUser: function(userId) {
            window.location.href = `/User/Edit/${userId}`;
        },

        /**
         * Delete user with confirmation
         * @param {number} userId - The user ID
         * @param {string} username - The username
         */
        deleteUser: function(userId, username) {
            if (modalTitle) modalTitle.innerText = 'Delete User';
            if (modalContent) {
                modalContent.innerHTML = `
                    <div class="text-center py-4">
                        <i class="fas fa-exclamation-triangle text-red-500 text-5xl mb-4"></i>
                        <p class="text-gray-700 mb-2">Are you sure you want to delete user <strong class="text-red-600">${escapeHtml(username)}</strong>?</p>
                        <p class="text-sm text-gray-500">This action cannot be undone. All associated data will be permanently removed.</p>
                    </div>
                `;
            }
            
            if (modalConfirmBtn) {
                modalConfirmBtn.innerText = 'Delete User';
                modalConfirmBtn.classList.remove('hidden');
                
                // Remove previous event listeners
                const newConfirmBtn = modalConfirmBtn.cloneNode(true);
                modalConfirmBtn.parentNode.replaceChild(newConfirmBtn, modalConfirmBtn);
                window.modalConfirmBtn = newConfirmBtn;
                
                newConfirmBtn.onclick = () => {
                    // Get anti-forgery token if needed
                    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
                    
                    fetch(`/User/Delete/${userId}`, { 
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': token
                        }
                    })
                    .then(response => {
                        if (response.ok) {
                            window.location.reload();
                        } else {
                            return response.text().then(text => {
                                alert('Failed to delete user: ' + text);
                            });
                        }
                    })
                    .catch(error => {
                        console.error('Error deleting user:', error);
                        alert('An error occurred while deleting the user. Please try again.');
                    });
                };
            }
            
            openModal();
        },

        /**
         * Export users data
         * @param {string} searchTerm - Optional search term to filter exported data
         */
        exportUsers: function(searchTerm = '') {
            const search = searchTerm || new URLSearchParams(window.location.search).get('search') || '';
            window.location.href = `/User/Export?search=${encodeURIComponent(search)}`;
        },

        /**
         * Close the modal dialog
         */
        closeModal: function() {
            closeModal();
        },

        /**
         * Refresh the user table (reloads the page)
         */
        refreshTable: function() {
            window.location.reload();
        },

        /**
         * Change page size and reload
         * @param {number} pageSize - The new page size
         */
        changePageSize: function(pageSize) {
            const currentUrl = new URL(window.location.href);
            currentUrl.searchParams.set('pageSize', pageSize);
            currentUrl.searchParams.set('page', 1);
            window.location.href = currentUrl.toString();
        },

        /**
         * Go to specific page
         * @param {number} pageNumber - The page number to navigate to
         */
        goToPage: function(pageNumber) {
            const currentUrl = new URL(window.location.href);
            currentUrl.searchParams.set('page', pageNumber);
            window.location.href = currentUrl.toString();
        },

        /**
         * Search users
         * @param {string} searchTerm - The search term
         */
        searchUsers: function(searchTerm) {
            const currentUrl = new URL(window.location.href);
            if (searchTerm) {
                currentUrl.searchParams.set('search', searchTerm);
            } else {
                currentUrl.searchParams.delete('search');
            }
            currentUrl.searchParams.set('page', 1);
            window.location.href = currentUrl.toString();
        },

        /**
         * Reset search filters
         */
        resetSearch: function() {
            const currentUrl = new URL(window.location.href);
            currentUrl.searchParams.delete('search');
            currentUrl.searchParams.set('page', 1);
            window.location.href = currentUrl.toString();
        }
    };
})();

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        UserManagement.init();
    });
} else {
    UserManagement.init();
}

// Make UserManagement available globally
window.UserManagement = UserManagement;