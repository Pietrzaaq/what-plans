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
            tooltipText: null
        };
    },
    mounted() {
        this.checkOverflow();
    },
    methods: {
        checkOverflow() {
            const container = this.$refs.container;
            if (container.scrollWidth > container.clientWidth) {
                this.tooltipText = this.text;
            } else {
                this.tooltipText = null;
            }
        },
    },
};
</script>

<template>
    <div ref="container" class="overflow-container" v-tooltip="tooltipText" @mouseover="checkOverflow">
        <slot>
            {{ text }}
        </slot>
    </div>
</template>

<style scoped>
.overflow-container {
    display: inline-block;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    text-wrap: nowrap;
    width: 100%;
}
</style>
