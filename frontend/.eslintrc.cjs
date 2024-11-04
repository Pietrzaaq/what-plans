/* eslint-env node */
require('@rushstack/eslint-patch/modern-module-resolution');

module.exports = {
    root: true,
    extends: ['eslint:recommended', 'plugin:vue/vue3-essential'],
    parserOptions: {
        ecmaVersion: 'latest'
    },
    ignorePatterns: ['**/public/**',  '**/external_modules/**'],
    rules: {
        'vue/multi-word-component-names': 'off',
        'vue/no-reserved-component-names': 'off',
        'vue/component-tags-order': [
            'error',
            {
                order: ['script', 'template', 'style']
            }
        ],
        'semi': [2, "always"],
        'vue/first-attribute-linebreak': ["warn", {
            "singleline": "beside",
            "multiline": "beside"
        }],
        "vue/html-indent": ["error", 4, {
            "attribute": 1,
            "baseIndent": 1,
            "closeBracket": 0,
            "alignAttributesVertically": true,
            "ignores": []
        }],
        "vue/html-closing-bracket-newline": ["error", {
            "singleline": "never",
            "multiline": "never"
        }],
        "object-curly-spacing": ['error', 'always']
    }
};
