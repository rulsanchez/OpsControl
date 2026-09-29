type PriorityBadgeProps = {
    priority: string
}

const labels: Record<string, string> = {
    Low: 'Baja',
    Medium: 'Media',
    High: 'Alta',
    Critical: 'Crítica'
}

export default function PriorityBadge({ priority }: PriorityBadgeProps) {
    return (
        <span className={`priority-${priority.toLowerCase()}`}>
            {labels[priority] ?? priority}
        </span>
    )
}
