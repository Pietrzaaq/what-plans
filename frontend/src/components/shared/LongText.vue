<script>
export default {
    props: {
        text: {
            type: String,
            required: true,
        },
    },
    data() {
        return {
            tooltipText: null, // Tooltip content, null if not needed
        };
    },
    mounted() {
        this.checkOverflow();
    },
    methods: {
        checkOverflow() {
            const container = this.$refs.container;
            if (container.scrollWidth > container.clientWidth) {
                this.tooltipText = this.text; // Text is overflowing, set tooltip
            } else {
                this.tooltipText = null; // No overflow, hide tooltip
            }
        },
    },
};
</script>

<template>
    <div ref="container" class="overflow-container" v-tooltip="tooltipText" @mouseover="checkOverflow">
        <span>{{ text }}</span>
    </div>
</template>

<style scoped>
.overflow-container {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    width: 100%; /* Adjust width according to parent or layout */
}
</style>
